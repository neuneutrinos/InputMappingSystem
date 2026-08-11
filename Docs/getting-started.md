# Getting Started

## Prerequisites

- Copy `addons/InputMappingSystem/` into your Godot project:
  `res://addons/InputMappingSystem/`
- Plugin activation is required.

To activate the plugin, select **Project → Project Settings → Plugins** and enable it.

## Create a NamedInputAction

Create a new Resource in your project.

Right-click on a folder → **Create New... → Resource** and search for `NamedInputAction`.

If it is not visible, the plugin may be disabled. Check that the plugin is enabled.

- Name it as you want. It is recommended that the name matches the corresponding Godot input action name.

- In the **Event** property, select an `InputEvent`, for instance `InputEventKey` for keyboard input.

- You can configure and automatically fill the event data. You can use either the keycode or the physical keycode.

- The **Pressed** property is important and should be enabled.

- Once the event is configured, name your action using the **Action Name** property.

## Create an InputMapping

Create a new Resource in your project.

Right-click on a folder → **Create New... → Resource** and search for `InputActionMapping`.

- Name it as you want. It is recommended that the name clearly describes the context.

- Add all the `NamedInputAction` resources you need for this context.

- Set a name for the context using the **Mapping Name** property.

## Create a NodeInputActionMappingContext

Add this node to your scene.

Add all the input mappings you need.

Connect the `OnActionTriggered` signal to the node that should handle the input.

From a node in your scene, activate the desired context.

And... voilà!
