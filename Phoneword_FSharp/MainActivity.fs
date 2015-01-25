namespace Phoneword_FSharp

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget

open System.Resources

[<Activity (Label = "Phoneword", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()

    let mutable translatedNumber = String.Empty

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resource_Layout.Main)

        // Get our UI controls from the loaded layout
        let view = this.LayoutInflater.Inflate (Resource_Layout.Main, null)
        let phoneNumberText = this.FindViewById<EditText> (Phoneword_FSharp.Resource_Id.PhoneNumberText)
        let translateButton = this.FindViewById<Button> (Phoneword_FSharp.Resource_Id.TranslateButton)
        let callButton = this.FindViewById<Button> (Phoneword_FSharp.Resource_Id.CallButton)

        // Disable the "Call" button
        callButton.Enabled <- false

        translateButton.Click.Add(fun _ ->
            // Translate user's alphanumeric phone number to numeric
            translatedNumber <- Translator.toNumber (phoneNumberText.Text)

            if String.IsNullOrWhiteSpace translatedNumber
                then
                    callButton.Text <- "Call"
                    callButton.Enabled <- false
                else
                    callButton.Text <- "Call " + translatedNumber
                    callButton.Enabled <- true
        )

        callButton.Click.Add(fun _ -> 
            let callDialog = new AlertDialog.Builder (this)

            let startCall _ _ =
                let callIntent = new Intent (Intent.ActionCall)
                callIntent.SetData (Android.Net.Uri.Parse <| "tel:" + translatedNumber) 
                |> this.StartActivity

            ignore <| callDialog.SetMessage ("Call " + translatedNumber + "?") 
            ignore <| callDialog.SetNeutralButton ("Call", startCall)
            ignore <| callDialog.SetNegativeButton ("Cancel", fun _ _ -> ())
            ignore <| callDialog.Show () 
        )
