# Unity_ECS_Samples
Collection of DOTS Entity Component System exercises and a basic ECS netcode setup.
Created on Unity 6000.0.17f1

# Contents

## Project 1
A capsule that moves to a destination.
- Intro to Components, Authoring, Baking, and Systems. 

## Project 2
Our capsule now moves to the nearest `MovementTarget`
- Intro to [System Update Order](https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/systems-update-order.html)
- Intro to `SystemAPI.Query`

## Project 3
Player-controlled ECS agent.
- Passes input from Unity's [Input System Package](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.9/manual/index.html) to ECS simulation.
- Adapted from [this sample](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/d616c1b077c306e6f31b41a3970799e4b132139b/UnityPhysicsSamples/Assets/Common/Scripts/DemoInputGatheringSystem.cs)

## Project 4
Continuously spawns huge amounts of Actors with health. Implements a global damage-over-time that will kill the Actors.
- Intro to Jobs (multithreading)
- Intro to [Entity Command Buffers](https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/systems-entity-command-buffers.html) for thread-safe scheduling

## Networked Cube
Literally just [this tutorial](https://docs.unity3d.com/Packages/com.unity.netcode@1.3/manual/networked-cube.html)
* NOTE: The `ClientServerBootstrap` for this project is commented out! Since only one bootstrap can live in a project, you'll need to swap them in [Game.cs](https://github.com/Jared-Freeman/Unity_ECS_Samples/blob/main/Assets/Scripts/Networking/Common/Game.cs)

## Connect to Server
Implements client/server hosting for our Networked Cube scene. Build settings are configured so you can build this one out, port forward, and have players connect remotely.
- This project mostly aggregates and iterates on code from UnityTechnologies.
- Artwork (if you can call it that) by me

# Resources

- [Entity Component System Samples](https://github.com/Unity-Technologies/EntityComponentSystemSamples)
- [ECS Networked Racing](https://github.com/Unity-Technologies/ECS-Network-Racing-Sample)
- [Megacity Metro](https://github.com/Unity-Technologies/megacity-metro/tree/master)