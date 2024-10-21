global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.Specialized;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Dynamic;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Nodes;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Web;
global using System.Windows.Media.Imaging;

global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using DependencyPropertyGenerator;

global using Microsoft.Win32;

global using OtomadHelper.Helpers;
global using OtomadHelper.WPF.Common;

global using static OtomadHelper.Helpers.Debugger;
global using static OtomadHelper.Helpers.Extensions;
global using static OtomadHelper.Helpers.I18n;
global using static OtomadHelper.Helpers.Misc;
global using static OtomadHelper.Interop.PInvoke;
global using static OtomadHelper.Services.MessageSender;

global using DialogResult = System.Windows.Forms.DialogResult;
global using ObservableObject = OtomadHelper.WPF.Common.ObservableObject;
global using Path = OtomadHelper.Helpers.Path;
global using Range = (double Min, double Max);
global using Rect = System.Windows.Rect;
global using ResourceDictionaries = System.Collections.ObjectModel.Collection<System.Windows.ResourceDictionary>;
global using Screen = System.Windows.Forms.Screen;
