# Json Transformer aka JsonT

This .Net Core tool is used to add or remove the section(s) in respective json file. This file could be a appsetting or any json configuration file for that matter.

The tool needs a settings file, a json; defining the section(s) that need to be updated in the respective configuration file(s).

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
  -s, --sourcepath:             Provide the source path to the config file(s)
  -c, --config:                 Provide full path to JsonT config file
```

## Getting started

**_Creating settings file_**<br/>
A setting file defines sections that help the JsonT to update the configuration file(s).

_Template_:

```
[
	{
		FileName: '<file-to-update>',
		Sections: {
			'<configuration>': 'value',
		},
	},
	{
		FileName: '<file-to-update>',
		Sections: {
			'<configuration>': 'value',
		},
	},
];
```

_FileName_: Provid name of the file to update; a json file.<br/>
ex: `JsonT.json`
<br/>

_Sections_: Provide the configuration detail(s). The section is dictionary that takes `configuration` and `value`. The `configuration` can heirarchy of objects seperated by '`:`'.<br/>
ex:

```
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
