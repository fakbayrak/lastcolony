# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**LastColony** is an isometric colony/building simulation game built with **Unity 6000.3.10f1 (Unity 6)**. The project is in early prototype stage with core grid, camera, and building placement systems implemented.

## Unity Development

This is a Unity project. There are no CLI build/test commands — all compilation, testing, and running happens inside the Unity Editor. To work on this project:

- Open `LastColony/` as the Unity project folder in Unity Hub (version 6000.3.10f1)
- The main scene is `Assets/Scenes/SampleScene.unity`
- Scripts compile automatically when saved; check the Unity console for errors

The project uses the **New Input System** (`com.unity.inputsystem`), not the legacy Input class.

## Architecture

### Core Systems (Assets/Scripts/)

**GameManager** (`Core/GameManager.cs`) — Singleton with `DontDestroyOnLoad`. Currently a shell; intended as the central game state controller.

**GridManager** (`Core/GridManager.cs`) — Owns the 20×20 tile grid. Tracks cell occupancy via a 2D `GridCell[,]` array. Provides coordinate conversion between world space and grid space, bounds checking, and occupancy management. Draws the grid in the editor via `OnDrawGizmos`. Other systems should go through GridManager for any grid state changes.

**IsometricCamera** (`Core/IsometricCamera.cs`) — Controls the camera with WASD/arrow key panning and scroll-wheel zoom (orthographic size clamped 5–20). Camera is fixed at 45°/45° rotation for isometric perspective. Reads input via the new InputSystem.

**BuildingPlacement** (`Buildings/BuildingPlacement.cs`) — Handles interactive building placement. Uses raycasting against a ground plane to determine the grid cell under the cursor. Shows a highlight quad (green = valid, red = occupied) during placement mode. Calls `GridManager` to validate and mark cells on placement. Exposes `EnterPlacementMode()` and `ExitPlacementMode()` as public API.

### Empty Placeholder Directories

`Scripts/NPC/`, `Scripts/Resources/`, `Scripts/UI/` — scaffolded but empty; planned for future systems.

### Prefabs

`Assets/Prefabs/CubePrefab.prefab` — Default building object. `BuildingPlacement` falls back to creating a primitive cube if no prefab is assigned in the Inspector.

## Dosya Konumları

- Core scriptler: `Assets/Scripts/Core/`
- Bina scriptleri: `Assets/Scripts/Buildings/`
- NPC scriptleri: `Assets/Scripts/NPC/`
- Kaynak scriptleri: `Assets/Scripts/Resources/`
- UI scriptleri: `Assets/Scripts/UI/`

## C# Kod Stili

- Private değişkenler camelCase: örnek → `gridManager`
- Public değişkenler PascalCase: örnek → `GridManager`
- Inspector'da görünmesi gereken private değişkenler `[SerializeField]` kullan
- `FindObjectOfType` kullanma; referansları Inspector'dan bağla

## Git Commit Formatı

- `Add X - açıklama` — yeni özellik eklerken
- `Fix X - açıklama` — hata düzeltirken
- `Update X - açıklama` — mevcut özelliği güncellerken

## Key Conventions

- **Singleton pattern**: GameManager uses a static `Instance` reference set in `Awake`.
- **Grid coordinates**: Always `Vector2Int` (column, row); world positions are `Vector3`. Use `GridManager.WorldToGrid()` / `GridToWorld()` for conversion — do not compute this ad hoc.
- **Placement mode entry point**: Call `BuildingPlacement.EnterPlacementMode()` from UI or other systems; never manipulate its internal state directly.
- **Material transparency**: `BuildingPlacement.SetMaterialTransparent()` manually sets shader blend mode properties — this is required because Unity's Standard shader doesn't expose transparency through a simple property at runtime.
