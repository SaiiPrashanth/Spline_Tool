# Spline Tool

A **Unity** `MonoBehaviour` that distributes any prefab along a parabolic spline between two anchor points, with real-time Scene view gizmos and automatic path alignment.

## Demo

![Demo](SplineTool.mp4)

## Overview

Manually placing repeated objects along curved paths — fence posts, lamp posts, chains, cables — is tedious. Attach this component to a GameObject, assign two `Transform` anchors and a prefab, and the tool instantly generates the entire chain in the scene, updating live as you move the anchors.

## Features

- **Live Distribution**: Spawns `Count` instances of a prefab along the path. Updates in real time in Edit mode (`[ExecuteAlways]`).
- **Parabolic Sag**: Adds a configurable downward curve (`Sag`) to simulate gravity on cables, wires, or ropes.
- **Path Alignment**: Optionally rotates each prefab to face the direction of travel along the spline.
- **Rotation & Scale Overrides**: Apply a per-instance `RotationOffset` and uniform `ScaleOverride`.
- **Auto Update**: Toggle whether the path rebuilds every frame or only on demand.
- **Context Menu Actions**: Right-click the component to **Rebuild** or **Clear** the spline without entering Play mode.
- **Scene Gizmos**: Cyan spheres mark the start/end points; the sag curve is drawn in wireframe for easy authoring.

## Prerequisites

- **Unity** (2021.3 LTS or newer recommended).
- Works in both Edit and Play mode.

## Usage

1. Add `Scripts/SplineTool.cs` to your Unity project (e.g., `Assets/Scripts/`).
2. Create an empty GameObject and attach the `SplineTool` component.
3. Assign **Start Point** and **End Point** transforms in the Inspector.
4. Assign the **Prefab** you want to distribute.
5. Adjust **Count**, **Sag**, **Rotation Offset**, and **Scale Override** to taste.
6. The spline rebuilds automatically when `AutoUpdate` is enabled, or use the **Rebuild** context menu entry.

## Inspector Reference

| Property | Default | Description |
|---|---|---|
| `StartPoint` | — | Transform marking the spline start |
| `EndPoint` | — | Transform marking the spline end |
| `Prefab` | — | GameObject to instance along the path |
| `Count` | `10` | Number of prefab instances (1 – 100) |
| `Sag` | `0.3` | Parabolic sag depth at the midpoint (0 – 2) |
| `RotationOffset` | `(0,0,0)` | Euler offset applied after path alignment |
| `ScaleOverride` | `(1,1,1)` | Uniform scale for every instance |
| `AutoUpdate` | On | Rebuild every `Update()` call |
| `AlignToPath` | On | Rotate instances to face the spline direction |

## Script Reference

### `Scripts/SplineTool.cs`
- **`Rebuild()`**: Clears children, then spawns prefabs at sag-interpolated positions between StartPoint and EndPoint. Aligns each to the local tangent when `AlignToPath` is enabled.
- **`Clear()`**: Destroys all child GameObjects (supports both Edit and Play mode).
- **`OnDrawGizmos()`**: Draws the endpoint markers and the 20-segment sag curve in the Scene view.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
Copyright (c) 2025 ARGUS
