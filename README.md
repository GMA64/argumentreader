[![Version: 1.0 Release](https://img.shields.io/badge/Version-1.0%20Release-green.svg)](https://github.com/GMA64) [![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Build Status](https://travis-ci.org/GMA64/argumentread.svg?branch=master)](https://travis-ci.org/GMA64/argumentread)
# ArgumentReader
---

### Description

With Argument Reader command line argumets can be passed into a .net core application. The standard project assist 4 types of argumnets:
* Boolean
* Strings *(\*)*
* Integer *(#)*
* Doubles *(##)*

Own argument tyoes can be build with own classes. They need to inherit form the 
**ArgumentMarshalerLib**. Libraries are loading dynamicaly on startup. It is not necessary to recompile the complete solution. 

---

## Structure

```csharp
Arguments paramenter = new Arguments("Path to Marshaler Liraries", "Schema", "Arugment Array")
```

### Available Marshalers (Standards)
- [BooleanMarshalerLib.dll](https://github.com/GMA64/argumentreader/releases/latest/download/BooleanMarshalerLib.dll)
- [StringMarsahlerLib.dll](https://github.com/GMA64/argumentreader/releases/latest/download/StringMarsahlerLib.dll)
- [IntegerMarshalerLib.dll]((https://github.com/GMA64/argumentreader/releases/latest/download/IntegerMarshalerLib.dll))
- [DoubleMarshalerLib.dll](https://github.com/GMA64/argumentreader/releases/latest/download/DoubleMarshalerLib.dll)

### Schema

1. Parameter name
1. Marshaler type

### Example

```csharp
Arguments parameter = new Arguments("...", "enabled, text*, number#, decimal##")

enabled b = parameter.GetValue<bool>("enabled") // false
text b = parameter.GetValue<string>("text") // false
number b = parameter.GetValue<int>("number") // false
decimal b = parameter.GetValue<double>("decimal") // false
```


---

### Boolean:

```bash
Arguments.exe -a -b
```

```csharp
static void Main(string[] args)
{
    parameter = new Arguments(@".\Marshaler", "a, b*", args);

    bool a = parameter.GetValue<bool>("a") // true
    bool b = parameter.GetValue<bool>("b") // false
    //...
}  
```

---

### String:

```bash
Arguments.exe -a "This is a text"
```

```csharp
static void Main(string[] args)
{
    parameter = new Arguments(@".\Marshaler", "a*", args);

    string a = parameter.GetValue<string>("a") // This is a Text
    //...
}  
```

---

### Integer:

```bash
Arguments.exe -a 1234
```

```csharp
static void Main(string[] args)
{
    parameter = new Arguments(@".\Marshaler", "a#", args);

    int a = parameter.GetValue<int>("a") // 1234
    //...
}  
```

---

### Double:

```bash
Arguments.exe -a 123,456
```

```csharp
static void Main(string[] args)
{
    parameter = new Arguments(@".\Marshaler", "a##", args);

    double a = parameter.GetValue<double>("a") // 123,456
    //...
}  
```

---

## Build your own Marshaler

1. Create a new VisualStudio .net Standard Class (**??MarshalerLib**)
1. Link a new projekt reference to ArgumentMarshalerLib.dll (in this repository)
1. Write Marshaler (See example code below)
1. Copy the TestMarshalerLib.dll to the Marshaler directory in your project
1. Implement the *?* in your schema (e.g "mymarshaler?")

```csharp

public class TestMarshalerLib : ArgumentMarshalerLib.ArgumentMarshaler
    {
        // Only schemas allowed that are not used (string.empty, *, #, ## are already used form standard marshalers)
        public override string Schema => "?";

        public override void Set(Iterator<string> currentArgument)
        {
            try
            {
                // If implementation should be use an argument behind the command (e.g -a ??),
                // it is neccassary to move the Iterator to the nex position
                Value = currentArgument.Next();

            }
            catch (ArgumentOutOfRangeException)
            {
                throw new TestgMarshalerException(ErrorCode.MISSING);
            }
                // If no argument behind the command is used just add your value
                Value = "This is my personal marshaler";
        }

        public class TestgMarshalerException : ArgumentMarshalerLib.ArgumentsException
        {
            public TestgMarshalerException() { }

            public TestgMarshalerException(ErrorCode errorCode) : base(errorCode) { }

            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    case ErrorCode.MISSING:
                        return $"Could not find string parameter for -{ErrorArgumentId}";
                    default:
                        return string.Empty;
                }
            }
        }
    }

``` 

---

## Reference

The original Argument Marshaler was written in Java and published by Robert c. Martin in his book Clean Code. This project adapt his implementation and extends it dynamically.



