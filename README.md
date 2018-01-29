
## versioner

Utility to modify version for a .NET Project.

###### What it does?

Generates a new version with the definition below and modifies all related files for the given project. 

```
 <Major>.<Minor>.<Build>.<Revision>

  Major : as the given parameter
  Minor : as the given parameter
  Build : calculates how many days passed since the first day of the given year
  Revision : the hour of versioning operation 
```

###### Example Usage

```
 versioner.exe -m 3 -n 2 -y 2015 -p "c:\project1\project1.csproj" 

   -m : major version 
   -n : minor version 
   -y : birth year of the project 
   -p : project file complete path 
```











