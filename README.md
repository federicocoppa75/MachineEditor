# MachineEditor
This solution contains projects for the analysis of the movement of CNC machines. The machine is represented by a tree structure that determines the relationship between the various elements (mesh - .stp file); through this structure it is possible to view the dimensions of the machine elements and their interaction even during movements.

## MachineEditor
Editor per le macchine, permette la creazione e la modifica.

[comment]:##MachineModels
[comment]:##MachineModels.IO

## MachineSteps.Editor
Machine movement editor

## MachineSteps.IsoInterpreter.SimpleApp
Application for converting the ISO for cx100 into machine steps

[comment]:##MachineSteps.Models
[comment]:##MachineSteps.Plugins.IsoConverterBase
[comment]:##MachineSteps.Plugins.IsoInterpreter
[comment]:##MachineSteps.Plugins.IsoIstructionAttributes
[comment]:##MachineSteps.Plugins.IsoIstructions
[comment]:##MachineSteps.Plugins.IsoParser
[comment]:##MachineSteps.Plugins.StepsViewer

## MachineSteps.Viewer
Application for viewing machine movements

[comment]:##MachineViewer
[comment]:##MachineViewer.Plugins.Common
[comment]:##MachineViewer.Plugins.Injectors.SimpleManipolator
[comment]:##MachineViewer.Plugins.Links.SimpleManipolator
[comment]:##MachineViewer.Plugins.Panel.MaterialRemoval
[comment]:##MachineViewer.Plugins.Panel.SimpleManipolator
[comment]:##MachineViewer.Plugins.ToolChange.SimpleManipolator
[comment]:##MachineViewer.Plugins.Tooling.SimpleManipolator

## MachineViewer.SimpleApp
Test application for various machine model display functions.

[comment]:##MachineViewer.SystemsAssembler
[comment]:##MachineViewModels
[comment]:##MachineViewModelUtils

## ModelSemplifier
Application for the simplification of models (mesh - .stl file) of the machine parts.

## TestIsoParser
Driver application for the MachineSteps.Plugins.IsoParser module.

[comment]:##TestMaterialRemoval
[comment]:##TestMovePanel
[comment]:##TestTrasform

## ToolEditor
Tools editor.

## ToolingEditor
Tooling editor.
