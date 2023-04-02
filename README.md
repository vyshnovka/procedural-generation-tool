# Procedural Terrain Generation

The project contains some algorithms for procedural terrain generation implemented on C# for Unity.    
Algorithms generate a heightmap, then a texture and a terrain based on it.

### Perlin Noise

Perlin noise is a type of gradient noise. It is most basic yet most customizable algorithm. Proper settings allow to achieve absolutely unique textures.

<img width="731" alt="image" src="https://user-images.githubusercontent.com/70700078/229382819-c169cbf1-fb1e-487f-aaa2-507718b5ccf6.png">

### Diamond-Square

Diamond-Square algorithm is suitable for generating realistic-looking landscapes. It was once described as flawed due to noticeable vertical and horizontal creases at the edges, but who cares if it still generates a decent result?

<img width="730" alt="image" src="https://user-images.githubusercontent.com/70700078/229382830-17872393-813a-42b9-b0fd-5296a9d82cbf.png">

### Worley Noise

Worley noise comes close to simulating textures of stone, water, or biological cells. Seriously, doesn't it look like sea waves?

<img width="731" alt="image" src="https://user-images.githubusercontent.com/70700078/229382836-c09113be-92d9-49d3-adec-479d1e4ea768.png">

 ## Tools

![image](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white) 
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white) 

## Setup

1. Install Unity 2021.3.21 from [archive](https://unity3d.com/get-unity/download/archive).    
2. Clone this repository using `git clone https://github.com/vyshnovka/procedural-generation.git` in Git Bash.    
3. Open created folder as a project in Unity Hub.    
4. Don't forget to star!
