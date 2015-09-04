# Lx.Tools
Lx.Tools is a set of tools developped by Leoxia (http://www.leoxia.com)

## Lx.Tools.Projects.Sync

It's a command line tool used to synchronize csproj file with an existing file 
named <name_of_the_csproj>.dll.sources which contains path to sources files.
Use case for this tool is mono source code where the .dll.sources files 
are used by Makefiles and then csproj are othen desynchronized in terms of which files 
needs to be compiled to create a given assembly.

## Lx.Tools.Projects.SourceDump

It's a simple command line tool used to dump on console or in a file (depending on the command line
arguments), the compiled sources of csproj file.

## Lx.Tools.Assemblies.Compare
 
It's a command line tool taking two assembly names and load the related assemblies. It then outputs
the differences in the public api of both. The plan is to use this tool on Mono vs .Net, in order to find out
where the main differences remain.

## Lx.Tools.Tests.Reflection

It's a set of helper classes to facilitate unit testing of classes.