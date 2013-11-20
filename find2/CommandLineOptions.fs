﻿namespace find2

open System
open CommandLine
open CommandLine.Text

[<SealedAttribute>]
type internal CommandLineOptions() =

    // a lot of mutable stuff
    // just to fit integrate well with CommandLineParser lib

    [<Option('t'
        , "textPattern"
        , DefaultValue = ""
        , HelpText = "Text pattern to search. Case insensitive.")>]
    member val TextPattern = "" with get, set

    [<Option('r'
        , "textRegexPattern"
        , DefaultValue = ""
        , HelpText = "Text regex pattern to search. Case insensitive.")>]
    member val TextRegexPattern = "" with get, set

    [<Option('f'
        , "fileNamePattern"
        , DefaultValue = ""
        , HelpText = "File name pattern to search. Case insensitive.")>]
    member val FileNamePattern = "" with get, set

    [<Option('x'
        , "fileNameRegexPattern"
        , DefaultValue = ""
        , HelpText = "File regex pattern to search. Case insensitive.")>]
    member val FileNameRegexPattern = "" with get, set

    [<Option('w'
        , "workingDirectory"
        , DefaultValue = ""
        , HelpText = "Working directory to start search with.")>]
    member val WorkingDirectory = "" with get, set

    [<Option('g'
        , "greps"
        , DefaultValue = ""
        , HelpText = "Greps string to filter on file names.")>]
    member val GrepsString = "" with get, set

    [<Option('l'
        , "matchLargeFiles"
        , DefaultValue = false
        , HelpText = "Examine contents of large files or not. False by default.")>]
    member val MatchLargeFiles = false with get, set

    member this.IsTextPatternRegex with get() = not (String.IsNullOrEmpty(this.TextRegexPattern))
    member this.IsFileNamePatternRegex with get() = not (String.IsNullOrEmpty(this.FileNameRegexPattern))

    [<ParserStateAttribute>]
    member val LastParserState:IParserState = null with get, set

    [<HelpOptionAttribute>]
    member this.GetUsage() = 
        HelpText.AutoBuild(this, fun current -> HelpText.DefaultParsingErrorsHandler(this, current))

    static member ParseArguments (args:string[]) = 
        let options = CommandLineOptions()
          
        if CommandLine.Parser.Default.ParseArguments(args, options)
            then None
        elif  String.IsNullOrEmpty(options.FileNameRegexPattern) 
                && String.IsNullOrEmpty(options.FileNamePattern)
            then
                printfn "%s" (options.GetUsage().ToString())
                printfn "-f/--fileNamePattern and -x/--fileNameRegexPattern are not allowed to be used together."
                None
        elif not(String.IsNullOrEmpty(options.FileNameRegexPattern))
                && not(String.IsNullOrEmpty(options.FileNamePattern))
            then
                printfn "%s" (options.GetUsage().ToString())
                printfn "-t/--textPattern and -r/--textRegexPattern are not allowed to be used together."
                None
        else Some(options)