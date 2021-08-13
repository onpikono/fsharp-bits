// Wrapping and unwrapping the single-case union

type Quantity = Quantity of float
type Price = Price of float
type Discount = Discount of float

let totalPrice (Quantity q) (Price p) (Discount d) = q * p * (1. - d)

// The same using object style

type Quantity' =
    | Quantity' of float
    member this.Value =
        let (Quantity' value) = this
        value

type Price' =
    | Price' of float
    member this.Value =
        let (Price' value) = this
        value

type Discount' =
    | Discount' of float
    member this.Value = let (Discount' value) = this in value

let totalPrice' (quantity: Quantity') (price: Price') (discount: Discount') =
    quantity.Value
    * price.Value
    * (1. - discount.Value)

// Named tuples and their deconstruction

type Point3D = Point3D of x: int * y: int * z: int

Point3D(3, 4, 5)

let getZ (Point3D (z = zValue)) = zValue
let getXZ (Point3D (x = xValue; z = zValue)) = xValue, zValue


let getZ' (Point3D (_, _, z)) = z // Deconstructing the classic way (alternative)

// Deconstructing named tuples in objects

type Color = string

type Paint =
    | Paint' of volume: float * pigment: Color
    member me.Volume =
        let (Paint' (volume = value)) = me
        value

let paint = Paint'(2.5, "red")
paint.Volume

// Active patterns, oh boy!

let (|Even|Odd|) num = if num % 2 = 0 then Even else Odd

let testNumber num =
    match num with
    | Even -> printfn $"{num} is even."
    | Odd -> printfn $"{num} is odd."

[ 1 .. 10 ] |> List.iter testNumber

open System.Drawing

let (|RGB|) (color: Color) = (color.R, color.G, color.B)

let (|RGBA|) (color: Color) = (color.R, color.G, color.B, color.A)

let (|HSB|) (color: Color) =
    (color.GetHue(), color.GetSaturation(), color.GetBrightness())

let printRGB (color: Color) =
    match color with
    | RGB (r, g, b) -> printfn $"Red: {r}, Green: {g}, Blue: {b}"

let printRGBA (color: Color) =
    match color with
    | RGBA (r, g, b, a) -> printfn $"Red: {r}, Green: {g}, Blue: {b}, Alpha: {a}"

let printHSB (color: Color) =
    match color with
    | HSB (h, s, b) -> printfn $"Hue: {h}, Saturation: {s}, Brightness: {b}"

let printAll (color: Color) =
    printfn $"Color: {color}:"
    color |> printRGB
    color |> printRGBA
    color |> printHSB

printAll Color.Red