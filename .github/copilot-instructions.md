# GitHub Copilot Instructions for Best Mix (Continued) Mod

## Mod Overview and Purpose

Best Mix (Continued) is an updated version of pelador's mod, enhancing the quality of life (QoL) in RimWorld by introducing variance in how benches select their ingredients. This mod allows players to customize the selection criteria for ingredients, providing a wide range of options to optimize their game experience.

## Key Features and Systems

### Ingredient Selection
The mod introduces a variety of criteria for selecting ingredients, including:

- **Beauty**: Select ingredients with the highest or lowest beauty.
- **Insulation**: Select ingredients with the best insulation properties for heat or cold.
- **Ignition**: Select the most flammable ingredients.
- **Condition**: Select ingredients with the highest or lowest hit points.
- **Expiry**: Select the most perishable items.
- **Robustness**: Select ingredients with the most hit points.
- **Mass**: Select ingredients with the heaviest or lightest mass.
- **Protection**: Select ingredients with the best protection against blunt or sharp damage.
- **Temperature**: Select ingredients with the coldest or warmest ambient temperature.
- **Value**: Select ingredients with the cheapest or most expensive value.
- **Weapon Damage**: Select ingredients with the bluntest or sharpest weapon damage.

### Customizable Settings
Players can set global settings for ingredient selection or individual settings for specific bills. The mod also includes options to:

- Limit functionality to meal source benches.
- Adjust stock selection criteria, such as evaluating the whole map or considering valid storage locations.

### Maximum Bills
The mod sets the maximum number of bills for a billgiver to 125, ensuring better management of workbenches.

## Coding Patterns and Conventions

### C# Files
The mod is primarily written in C# and follows common coding conventions, including:

- **Namespaces**: Use `BestMix` as the primary namespace.
- **Classes and Methods**: Classes and methods are named descriptively to convey their purpose.
- **Mod Initialization**: The `BestMix_Initializer` class handles the initialization of the mod.
- **Settings**: The `Controller` class manages mod settings through the settings window.

### XML Integration
XML files are used to define mod-specific data and integrate with RimWorld's data system. Key files include:

- **Defs**: Define new or modified game elements.
- **Settings**: Store mod-specific settings.

## Harmony Patching

Harmony patching is used extensively in the mod to modify existing game behavior without altering the original source code. Key Harmony patches include:

- **Bill_DoInterface**: Patches the `DoInterface` method to add custom GUI elements for ingredient selection.
- **BillStack_DoListing**: Patches the `DoListing` method to modify the behavior of ingredient listing.

## Suggestions for Copilot

### General Recommendations
- **Code Quality**: Ensure that generated code adheres to the existing coding patterns and conventions.
- **Documentation**: Include comments and documentation to explain the purpose and functionality of new code.
- **Testing**: Test new features thoroughly to ensure compatibility and functionality within the mod.

### Specific Requests
- **Harmony Patches**: Generate Harmony patches for new features, ensuring they integrate seamlessly with existing code.
- **XML Integration**: Provide examples of XML files for defining new elements and integrating with RimWorld's data system.
- **Mod Settings**: Generate code for new mod settings, including GUI elements for user interaction.

By following these guidelines, you can ensure that contributions made using GitHub Copilot are consistent with the Best Mix (Continued) mod's standards and enhance its functionality effectively.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

