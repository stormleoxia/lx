# L X

Lx is a set of libraries developped by Leoxia (http://www.leoxia.com)

## Lx.Tools.Projects.Dump
	Display on console list of source file included in project file.

	Usage: source-dump [options] file.csproj[other.csproj]
	source-dump 0.9.0.0
	Copyright (C) 2015 Leoxia Ltd
	  --help            Display this text
	  --unix-paths      Convert source path to unix paths
	  --windows-paths   Convert source path to windows paths
	  --relative-paths  Convert absolute path to relative ones
	  --absolute-paths  Convert relative paths to absolute ones
	  --output-file     Writes output in a file <csproj>.dll.sources in the same directory of the project file

## Lx.Tools.Projects.Sync
	Read *.dll.sources in the same directory of a project file and used it to update list of sources included in it.
	Was developed primarily for Mono project.

	Usage: sync-prj [options] file.csproj | directory [other.csproj][otherdirectory]
	sync-prj 0.9.0.0
	Copyright (C) 2015 Leoxia Ltd
	  --help            Display this text

	Note: When there is several sources in the same directory, choice is done based pattern matching, using the name of the source and the project.
	Remark: 
		- It does not work well for generated sources. Still have to figure out why.
		- References are not managed. Need to parse Makefile to do that.