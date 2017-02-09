# jzeferino.XSAddin (v0.1)

[![Build status](https://ci.appveyor.com/api/projects/status/2s6v7xgxobnr64of?svg=true)](https://ci.appveyor.com/project/jzeferino/jzeferino-xsaddin)

<p align="center">
  <img src="https://github.com/jzeferino/jzeferino.XSAddin/blob/master/art/icon.png?raw=true"/>
</p>

jzeferino.XSAddin is personal project that create a Xamarin Studio add-in to let you create a Xamarin Cross Platform solution (Xamarin.Native) or (Xamarin.Forms).
When creating the project the addin will show you a custom wizard wich the user can select the application name, the application identifier and if he wants .gitignore, readme and cake.
It already creates some PCL (ViewModel, Service, Controller, Localisation and Tests) projects to allow the shared code to be separated in different layers using MVVM pattern.
It also allows you to create some file templates.

## Goal
The main goal of this addin is to create a fast and clean Xamarin Cross Platform solution (Xamarin.Native) or (Xamarin.Forms) ready to add nuget packages in need for that specific project.

## Installation
1. Open Xamarin Studio `Add-in Manager`
2. Switch to `Galary` Tab
  1. Type `jzeferino.XSAddin`
  2. Install Add-in

## Usage
1. Start a new solution
2. Select Cross Platform under jzeferino section
3. Select Cross Platform solution template
4. Fill the fields you want and press next
5. Set solution/project name and press next
6. Done

## Demo
<p align="center">
  <img src="https://github.com/jzeferino/jzeferino.XSAddin/blob/master/art/demo_template.gif?raw=true"/>
</p>

## Building (OSX)
1. Install Xamarin Studio
2. Install the Add-in `Add-in Maker`
3. Clone this repo
4. Run

### Notes:
* This addin was made to satisfy my personal needs.
* Only tested in OSX.

### TODO
See [TODO](TODO.md) file.

### License
[MIT Licence](LICENSE) 