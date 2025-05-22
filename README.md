# Afterwarp.SpriteEngine
The  SpriteEngine is a framework that aims to create 2D game quickly and easily.  
It's base on Afterwarp graphic engine:
https://afterwarp.io/   

The SpriteEngine is a  class/framework which handles Sprite addition, removal, and common behavior. The SpriteManager also manages automatically updated PositionedObjects. By default all entity instances are added to the SpriteManager to have their every-frame activity called.

Features:  
*Sprite Animation:  
You can have multiple animations for each sprite . You can tell a sprite to change animations, or to animate once (leaving the sprite displaying the final frame of the animation) 

*sprites with sprite animations, scrolling sprites, repeating sprites and sprite trails   
*Extensible rendering system. Add/remove Sprites as needed. Renderables are sorted by render layer.   

*Scrolling background,Collision,Movement,Rotation,Color adjust,Sprite Mirror/Flip,resize/scaling  
*Particle,Action game's Sprite jump class  
*Z-index sorting:Every sprite  has its own Z-index property to determine its rendering order.Also, you can dynamic change Sprite's z layer in real time.  
*Camera:Camera movement, Camera can resize and work on any  resolution.and also  follow the player around  
*sounds:sound  class for play music.Support mp3 midi,wave.  
*keyboard and mouse input  
*Overlay blend mode on sprites  
*Sprite image processing: Sprite's image  can be adjust Hue,saturation,brightness in real time.It's hardware level of image processing and without  FPS lost.     

*3 type of SpriteSheet mode:   
1.Single-display individual image sprites separately while using only a single image source file  
2.Pattern- Grid slicing sprite,collection of Sprites arranged in a grid if your sprites are all a fixed size.  
3.CropRect-Sprites are atlases images containing of non-sequential sprites.  
An atlas can consist of uniformly-sized images or images of varying dimensions.It's to grab only the region of the atlas.  

*Font TextOut(Aftwewarp native feature) : Render a high-quality text in 2D  using Signed Distance Fields (SDF) and Super-Sampling, which appear vector-like and remain readable even under steep viewing angles.   The text may be optionally filled with color gradients, with border and shadows. The framework includes full international support for Unicode text in UTF-8 standard.  
