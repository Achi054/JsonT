# Json Transformer aka JsonT

This .Net Core tool is used to update the content in respective json file. This file could be a appsetting or any json configuration file for that matter.

The tool needs a settings file, `.jt`; defining the content(s) that need to be updated in the respective file(s).

## Installation

```
dotnet tool install JsonT
```

## Usage

```
Usage: JsonT [options]

Options:
  help, --help                  Display help

Command Options:
  -s, --sourcepath:             Provide path to Json file(s)
```

## Getting started

**_Creating JsonT file_**<br/>
A _JsonT_ file defines sections that help the _JsonT_ to update the respective json file.The _JsonT_ file is json file with file extension `.jt`.<br/>

ex: <br/>
If the json file to update is `configuration.json` the respective JsonT file should be `configuration.json.jt`
<br/>

_Configurations_ should be a dictionary that takes `path` and `value`. The `path` can be a heirarchy of objects seperated by '`:`'.<br/>
ex:

```javascript
{
    "Auditing:Enable" : true,
    "Logging": {
        "Enable": true,
        "Level": "Error"
    },
    "Metrics:Settings" : {
        "Enable": true,
        "Format": "json"
    }
}
```

## Detailed build status

| Branch | Build Status                                                                                                       |
| ------ | ------------------------------------------------------------------------------------------------------------------ |
| dev    | ![GitHub Action Build](https://github.com/Achi054/JsonT/workflows/GitHub%20Action%20Build/badge.svg?branch=dev)    |
| master | ![GitHub Action Build](https://github.com/Achi054/JsonT/workflows/GitHub%20Action%20Build/badge.svg?branch=master) |
