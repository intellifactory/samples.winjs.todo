namespace winjs.todo

open WebSharper
open WebSharper.WinJS

[<JavaScript; Require(typeof<Resources.DarkTheme>)>]
module Client =
    
    open WebSharper.JQuery
    open WebSharper.JavaScript
    open WebSharper.Piglets
    open WebSharper.Html.Client

    type Task =
        {
            Text       : string
            LabelColor : string
            Tags       : string array
        }

    let ShowNotification text onAfterHide =
        let notification = JQuery.Of "#notification"
        
        notification
            .Text(text)
            .AddClass("visible")
            .Delay(900)
            .Queue(
                fun _ ->
                    notification
                        .RemoveClass("visible")
                        .Empty()
                        .Dequeue()
                    |> ignore

                    onAfterHide ()   
            )
        |> ignore

    let Main =
        
        let tasks = WinJS.Binding.List.Create [||]
        
        let keyword = ref ""

        let pivot = JS.Document.GetElementById "pivot"

        Application.OnReady <| fun () ->
            WinJS.Namespace.define(
                "Tasks",
                [
                    "Self" => tasks.createFiltered (fun task ->
                        Array.exists (fun (tag : string) -> tag.IndexOf !keyword > -1) task.Tags || task.Text.IndexOf !keyword > -1
                    )
                ]
                |> New
            )
            |> ignore

            (JS.Document.GetElementById "search").AddEventListener(
                "querychanged",
                fun (event : Dom.Event) ->
                    keyword := event?detail?queryText

                    tasks.notifyReload()
                ,
                false
            )

            (Div [
                Piglet.Return (fun text labelColor (tags : string) -> { Text = text; LabelColor = labelColor; Tags = tags.Split ',' })
                <*> Piglet.Yield "buy milk"
                <*> Piglet.Yield "#ffffff"
                <*> Piglet.Yield "milk, breakfast"
                |> Piglet.WithSubmit
                |> Piglet.Run (fun task ->
                    tasks.push task |> ignore

                    ShowNotification "Saved!" (fun () ->
                        pivot ? winControl ? selectedIndex <- 0
                    )
                )
                |> Piglet.Render (fun text labelColor tags x ->
                    Div [
                        Controls.Input text
                        |> Controls.WithLabel "Text"

                        Controls.Input labelColor
                        |> Controls.WithLabel "Label color"

                        Controls.Input tags
                        |> Controls.WithLabel "Tags"

                        Controls.Submit x -< [
                            Attr.Value "Save"
                        ]
                    ]
                )
            ])
                .AppendTo "newTask"

            JQuery.Of("#remove").Click(fun _ _ ->
                JQuery.Of(".task.selected").Each(fun task _ ->
                    tasks.splice(float <| JQuery.Of(task).Index(), 1.) |> ignore
                )
                |> ignore
            )
            |> ignore

            WinJS.UI.processAll()._done (
                fun _ ->
                    (JQuery.Of "body").Css("visibility", "visible") |> As
            )
        
        WinJS.Application.start()
