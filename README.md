# fsharp-bits

Bits and pieces of F# code picked up on the learning path.

## Set Up Debugging

This method allows stopping at breakpoints, unlike `dotnet [watch] run` which does not allow that.

1. Click `Run and Debug` button on the right panel or press `Ctrl-Shift-D`
2. Click `Start Debugging` or press `F5` key
3. Click the `...create a launch.json file.` link
4. Select `.Net Core`
5. Click `Add Configuration...` button
6. Select `{} .NET: Lauch .NET Core Console App`
7. Fix the path to the executable (the path can be determined by looking into `...\bin\Debug` folder):
   1. Replace `<target-framework>` with `net5.0` (or other actual folder name)
   2. Replace `project-name.dll/exe` with the name of the actual produced dll / exe file
8. Rerun `Start Debugging`
9. Click `Create Task`
10. Select `Create tasks.json file from template`
11. Select `.NET Core`
12. _Optional:_ Add command line argumets (`argv`) into the `lanuch.json` file, line `args`

## Run Program with "Hot Reload"

Type `dotnet watch run [args]` in the command line.

## Named Tuples Pattern Matching

Named tuples are single discriminated union tuples:

```fsharp
   type Point = x: int * y: int           // Does not compile - no name for the tuple
   type Point = Point of x: int * y: int  // Works!
```

```fsharp
   let x = (1,2)

   let x1,x2 = x                          // Normal tuple deconstruction at let-binding
   
   let xFunc (x1,x2) =                    // Normal tuple deconstruction in function let-binding
      x1 * x2
   
   let point = Point (3,4)
   let (Point(x,y)) = point               // Deconstruction of the union and the inner tuple
   
   let pointFunc (Point(x,y)) = ...      // Deconstruction as function input
```

But we are also able, in those union cases, to deconstruct only what we want by name (with a slightly different syntax, note the semicolon):

```fsharp
   let point = Point (5,6)
   let (Point(x = xaxis; y = yaxis)) = point
```

This is helpful in cases where we have 'large' tuples and in a given place are only interested in one part of the tuple:

```fsharp
   type Point3D = Point3D of x: int * y: int * z: int

   let point = Point3D (3, 4, 5)

   let getZ (Point3D (z = zValue)) = zValue
   
   let getXZ (Point3D (x = xValue; z = zValue)) = xValue, zValue
   let getXZ' (Point3D(x, _, z)) = x, z       // This is equivalent extraction, but clunky for large tuples
   getZ point     // 5
   getXZ point    // 3, 5
```

## Active Patterns

Not sure how we got here but never mind. From [https://docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns):

> Active patterns enable you to define named partitions that subdivide input data, so that you can use these names in a pattern matching expression just as you would for a discriminated union. You can use active patterns to decompose data in a customized manner for each partition.

Active patterns are actually functions and can be user like those too.

### Syntax

```fsharp
// Active pattern of one choice.
let (|identifier|) [arguments] valueToMatch = expression

// Active Pattern with multiple choices.
// Uses a FSharp.Core.Choice<_,...,_> based on the number of case names. In F#, the limitation n <= 7 applies.
let (|identifier1|identifier2|...|) valueToMatch = expression

// Partial active pattern definition.
// Uses a FSharp.Core.option<_> to represent if the type is satisfied at the call site.
let (|identifier|_|) [arguments] valueToMatch = expression
```

### Remarks

There can be up to seven partitions in an active pattern definition. The expression describes the form into which to decompose the data. You can use an active pattern definition to define the rules for determining which of the named partitions the values given as arguments belong to. The `(|` and `|)` symbols are referred to as banana clips and the function created by this type of *let binding* is called an *active recognizer*.

As an example, consider the following active pattern with an argument.

```fsharp
let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

let testNumber input =
   match input with
   | Even -> printfn $"{input} is even"
   | Odd -> printfn $"{input} is odd"
```
