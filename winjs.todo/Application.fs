namespace winjs.todo

open WebSharper
open WebSharper.JavaScript

[<JavaScript>]
type Application =
    
    [<Inline "WinJS.Application.onready = $0">]
    static member OnReady (a : unit -> 'a) = X<unit>
