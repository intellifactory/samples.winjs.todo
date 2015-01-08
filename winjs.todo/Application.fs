namespace winjs.todo

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.JavaScript

[<JavaScript>]
type Application =
    
    [<Inline "WinJS.Application.onready = $0">]
    static member OnReady (a : unit -> 'a) = X<unit>
