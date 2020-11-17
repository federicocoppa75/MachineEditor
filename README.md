# MachineEditor solution
This solution contains projects for the analysis of the movement of CNC machines. The machine is represented by a tree structure that determines the relationship between the various elements (mesh - .stp file); through this structure it is possible to view the dimensions of the machine elements and their interaction even during movements. The [**HelixToolkit.Wpf**](https://github.com/helix-toolkit/helix-toolkit/tree/master/Source/HelixToolkit.Wpf) library is used for 3D visualization. 

## Examples
This folder contains the files to simulate a simple 3 axis CNC being machined.
* **Simple3AxesCnc.xml**: machine structure, it could be edit by [MacineEditor](#MachineEditor) and load by [MachineViewer.SimpleApp](#MachineViewer.SimpleApp) or [MachineViewer](https://github.com/federicocoppa75/MachineSimulation.DX#machineviewer)
* **Models**: contains machene elements models (*.stl file)
* **SimpleToolSet.tools**: tools database, it could be edit by [ToolEditor](#ToolEditor)
* **SimpleTooling.tooling**: tooling example, it could be edit edit by [ToolingEditor](#ToolingEditor)
* **Simple3AxesCnc.mcfgx**: machine archive (contains machine structure and elements models, no link to other file with absolute path like *.xml machine struct), it could be edit by [MacineEditor](#MachineEditor) and load by [MachineViewer.SimpleApp](#MachineViewer.SimpleApp) or [MachineViewer](https://github.com/federicocoppa75/MachineSimulation.DX#machineviewer)
* **Simple3AxesCnc.env**: working environment (contains structure, elements model, tools and tooling), it could be save and load by [MachineViewer](https://github.com/federicocoppa75/MachineSimulation.DX#machineviewer)
* **antina.msteps**: example of machinary, it could be edit by [MachineSteps.Editor](#MachineSteps.Editor)

[![](./images/ShowExample.JPG)](https://www.youtube.com/watch?v=u2I6zB-JCqI)

## MachineEditor
Machine editor, allows creation and modification of machine struct. The machine struct could be save as simple *.xml file (it link machine elements models with absolute path) or archive *.mcfgx (it contains struct and models).

<!-- ##MachineModels
##MachineModels.IO -->

## MachineSteps.Editor
Machine movement editor (file *.msteps)

## MachineSteps.IsoInterpreter.SimpleApp
Application for converting the ISO for cx100 into machine steps

<!-- ## MachineSteps.Models
## MachineSteps.Plugins.IsoConverterBase
## MachineSteps.Plugins.IsoInterpreter
## MachineSteps.Plugins.IsoIstructionAttributes
## MachineSteps.Plugins.IsoIstructions
## MachineSteps.Plugins.IsoParser
## MachineSteps.Plugins.StepsViewer -->
## MachineSteps.Viewer
Application for viewing machine movements, it could load the machien struct (file *.xml or *.mcfgx) and relative tooling (file *.tooling) and then could show the machining (file *.msteps).

<!-- ## MachineViewer
## MachineViewer.Plugins.Common
## MachineViewer.Plugins.Injectors.SimpleManipolator
## MachineViewer.Plugins.Links.SimpleManipolator -->

## MachineViewer.Plugins.Panel.MaterialRemoval
This is the library that implements the material removal. It uses [**geometry3Sharp**](https://github.com/gradientspace/geometry3Sharp).

<!-- ## MachineViewer.Plugins.Panel.SimpleManipolator
## MachineViewer.Plugins.ToolChange.SimpleManipolator
## MachineViewer.Plugins.Tooling.SimpleManipolator -->

## MachineViewer.SimpleApp
Test application for various machine model display functions, it could load the machien struct (file *.xml or *.mcfgx) and relative tooling (file *.tooling).

<!-- ## MachineViewer.SystemsAssembler
## MachineViewModels
## MachineViewModelUtils -->

## ModelSemplifier
Application for the simplification of models (mesh - .stl file) of the machine parts.

## TestIsoParser
Driver application for the MachineSteps.Plugins.IsoParser module.

## TestMaterialRemoval
This video shows the test application for the material removal library.

[![](./images/TestMaterialRemoval.JPG)](https://www.youtube.com/watch?v=buKEhHzB6Eg)
<!-- [comment]:## TestMovePanel
[comment]:## TestTrasform -->

## ToolEditor
This is a tool to create a tool set that can be used to create a tooling (via [ToolingEditor](#ToolEditor)) that can then be loaded from a machine model loaded in [MachineViewer.SimpleApp](#MachineViewer.SimpleApp) or [MachineSteps.Viewer](#MachineSteps.Viewer).

![](./images/ToolEditor.JPG)

## ToolingEditor
This is a tool to create a tooling which can then be loaded from a machine model loaded in [MachineViewer.SimpleApp](#MachineViewer.SimpleApp) or [MachineSteps.Viewer](#MachineSteps.Viewer).

![](./images/ToolingEditor.JPG)
