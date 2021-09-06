#r "nuget: ExcelProvider"

open FSharp.Interop.Excel
open System

let [<Literal>] ExcelFilePath = __SOURCE_DIRECTORY__ + "/SAP_Users.xlsx"
type ExcelData = ExcelFile<ExcelFilePath>
let file = ExcelData()

let validUsers =
    file.Data
    |> Seq.toArray
    |> Array.filter (fun user -> user.``User valid to`` > System.DateTime.Today)

let inactiveUsers =
    validUsers
    |> Array.filter
        (fun user ->
            try
                DateTime
                    .Today
                    .Subtract(
                        user.``Last logon date``
                    )
                    .Days > 300
            with
            | _ex -> false)

let totalValidUsers = validUsers |> Array.length
let totalInactiveUsers = inactiveUsers |> Array.length

let countBy projection (users: ExcelData.Row array) =
    users
    |> Array.groupBy projection
    |> Array.map (fun (field, usersData) -> field, usersData |> Array.length)

let licenseCount =
    validUsers
    |> countBy (fun user -> user.``Short text for user types``.Trim())
    |> Array.sortBy (fun (_licenseType, count) -> count)

let userCountPerSite =
    validUsers
    |> countBy (fun user -> user.``User group``, user.``Short text for user types``.Trim())
    |> Array.sortBy (fun ((site, _licenseType), _count) -> site)
    |> Array.map (fun ((site, licenseType), count) -> site, licenseType, count)
