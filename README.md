# Procedural Terrain Generation

The project contains some algorithms for procedural terrain generation implemented on C# for Unity.    
Algorithms generate a heightmap, then a texture and a terrain based on it.

### White Noise

White noise represents a set of random values with no additional processing.

<img width="1200" alt="white-terrain" src="https://github.com/vyshnovka/procedural-generation/assets/70700078/3d485937-f42b-4cab-8b14-671c0010d144">


### Perlin Noise

Perlin noise is a type of gradient noise. It is most basic yet most customizable algorithm. Proper settings allow to achieve absolutely unique textures.

<img width="1201" alt="perlin-terrain" src="https://github.com/vyshnovka/procedural-generation/assets/70700078/54fd37e1-cc21-454e-8c84-3081bf2ac4df">

### Diamond-Square

Diamond-Square algorithm is suitable for generating realistic-looking landscapes. It was once described as flawed due to noticeable vertical and horizontal creases at the edges, but who cares if it still generates a decent result?

<img width="1201" alt="diamond-terrain" src="https://github.com/vyshnovka/procedural-generation/assets/70700078/2ac93025-1d7a-4ed5-9c2c-6b5f174795ea">

### Worley Noise

Worley noise comes close to simulating textures of stone, water, or biological cells. Seriously, doesn't it look like sea waves?

<img width="1200" alt="worley-terrain" src="https://github.com/vyshnovka/procedural-generation/assets/70700078/d965ea1c-461f-4f4f-a28e-1ce895380b0d">

 ## Tools

![image](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white) 
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white) 
![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## Setup

1. Install Unity 2022.3.19 from [archive](https://unity3d.com/get-unity/download/archive).    
2. Clone this repository using `git clone https://github.com/vyshnovka/procedural-generation.git` in Git Bash.    
3. Open created folder as a project in Unity Hub.
