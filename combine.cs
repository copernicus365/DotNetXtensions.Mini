#!/usr/bin/env dotnet script
#:package DotNetXtensions.Mini@1.0.1-beta-1
#nullable disable
// #:property TargetFramework=net10.0

// Script to combine all .cs files into a single file

using System.Text;
using System.Text.RegularExpressions;

using DotNetXtensions;

using static System.Console;

// Output file name
const string outputFile = "DnxMini.cs";

// Header block with usings and namespace
const string header = """
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetXtensions;

""";

// Parse command-line arguments
bool printHeaders = args.Contains("-nh") || args.Contains("--no-headers") ? false : true;

// Get all .cs files, excluding obj, bin folders, and the output file itself
string[] csFiles = [.. Directory.GetFiles("./DotNetXtensions.Mini/", "*.cs", SearchOption.AllDirectories)
	.Select(f => PathX.GetFullPath(f))
	.Where(f => !f.Contains($"/obj/") && !f.Contains($"/bin/") && !f.EndsWith(outputFile) && !f.EndsWith("/combine.cs"))
	 .OrderBy(f => f)];

FileWClassName[] allFileClasses = [.. csFiles.Select(GetFileWClassName).Where(f => f != null)];

IGrouping<string, FileWClassName>[] grps = [.. allFileClasses.GroupBy(f => f.ClassName)];

WriteLine($"Found {csFiles.Length} .cs files to combine:");
foreach(var file in csFiles) {
	WriteLine($"  - {Path.GetFileName(file)}");
}

StringBuilder sb = new(header);
sb.AppendLine();

// Process each class group
foreach(var group in grps) {
	string className = group.Key;
	FileWClassName[] files = [.. group];

	WriteLine($"Processing class: {className ?? $"?? {group}"} ({files.Length} file(s))");

	(string _content, string _header) = files.Length == 1
		? (files[0].Content, $"from file: {Path.GetFileName(files[0].Path)}")
		: (CombinePartialClasses(className, files), $"combined {files.Length} partial files for: {className}");

	if(printHeaders)
		sb.AppendLine($"\n// ========== {_header} ==========\n");
	sb.Append(_content);
	sb.Append("\n\n");
}

string finalContent = sb.ToString().Replace("\r\n", "\n");

// Write the combined file
File.WriteAllText(outputFile, finalContent, new UTF8Encoding(false));

WriteLine($"\nSuccessfully combined files into: {outputFile}");
WriteLine($"Total size: {new FileInfo(outputFile).Length} bytes");

static FileWClassName GetFileWClassName(string file)
{
	string content = File.ReadAllText(file);

	// Find the line after namespace declaration
	int nsIndex = content.IndexOf("namespace DotNetXtensions;");
	if(nsIndex < 0) {
		WriteLine($"Warning: Could not find namespace declaration in {Path.GetFileName(file)}");
		return null;
	}

	// Find the newline after the namespace declaration and get everything after
	int afterNs = content.IndexOf('\n', startIndex: nsIndex) + 1;
	string csContent = content.Substring(afterNs).Trim();

	// Find the specific line containing the class declaration

	string classLine = csContent.GetLinesLazy(trim: false, ignoreEmpty: true)
		.FirstOrDefault(line => line.Contains(" class "));

	if(classLine == null)
		throw new Exception($"Class declaration line couldn't be found: {file}");

	// Parse the class line to extract class name and partial flag
	var match = Regex.Match(classLine, @"\bclass\s+(\w+)");
	string className = match.Success ? match.Groups[1].Value : null;
	bool isPartial = classLine.Contains("partial");

	if(className == null)
		throw new Exception($"Class name couldn't be found in line: {classLine}");

	FileWClassName obj = new(className, isPartial, file, csContent);
	return obj;
}

static string CombinePartialClasses(string className, FileWClassName[] files)
{
	if(files.Length == 0) return "";
	if(files.Length == 1) return files[0].Content;

	StringBuilder sb = new();

	// Process first file - keep the class declaration
	var f = files[0];
	string first = f.Content;
	int firstClosingBrace = first.LastIndexOf('}');

	void appnd(int index, string fname)
		=> sb.AppendLine($"\n// ---\n// --- partial: {fname} ({index}) ---\n// ---\n\n");

	appnd(0, f.FileName);

	if(firstClosingBrace > 0) {
		sb.Append(first[..firstClosingBrace].TrimEnd());
		sb.Append("\n\n");
	}
	else {
		sb.Append(first);
	}

	// Process remaining files - extract content between class declaration and closing brace
	for(int i = 1; i < files.Length; i++) {
		FileWClassName ffile = files[i];
		string content = ffile.Content;

		// Find the opening brace of the class
		int openBrace = content.IndexOf('{');
		if(openBrace < 0) continue;

		// Find the last closing brace
		int closeBrace = content.LastIndexOf('}');
		if(closeBrace < 0) continue;

		// Extract the content between braces
		string innerContent = content.Substring(openBrace + 1, closeBrace - openBrace - 1).Trim();
		if(string.IsNullOrWhiteSpace(innerContent))
			continue;

		appnd(i, ffile.FileName);
		sb.Append(innerContent);
		sb.Append("\n\n");
	}

	// Add the final closing brace
	sb.Append("}");


	return sb.ToString();
}

record FileWClassName(string ClassName, bool IsPartial, string Path, string Content)
{
	public string FileName { get; init; } = System.IO.Path.GetFileName(Path);
}
