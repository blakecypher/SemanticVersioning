# SemanticVersioning

## Introduction
This task is intended for prospective .NET developers to demonstrate competency in the following areas:

Creating applications using well-written C# code
- JSON and file manipulation
- Writing tests
- Writing supporting documentation
## Exercise
The enclosed JSON file contains a version number in a semantic format (Major.Minor.Patch) where each part should be numeric.

The minor or patch number needs to be incremented depending on the type of release of the application:

- For a minor release, the minor number is incremented and the patch number is reset to zero.
- For a patch release, the patch number is incremented.
Please write:

A console application which takes as its argument the type of release of the application. It should read the version object from the file, increment the application’s version number according to the rules above, and save it back to the file, leaving any other contents unaltered.

Output
Your output should include:

- C# code to provide the solution
- Unit tests
- Appropriate documentation
- An accessible online code repository
