## 09c: Unity on Linux — NVIDIA 5060 Ti (16GB) Graphics Optimization

### Status: Ready to apply

### GPU Context
- **GPU**: NVIDIA 5060 Ti
- **VRAM**: 16 GB
- **OS**: Linux (proprietary NVIDIA driver recommended)

### Graphics API Settings
- Go to: Edit → Project Settings → Player → Other Settings
- **Graphics APIs**:
  - Set **Vulkan** as the first/primary API (preferred on Linux/NVIDIA)
  - Remove **OpenGL** if not needed, or keep as a fallback

### Quality Settings
- Go to: Edit → Project Settings → Quality
- **Enable**: GPU Skinning
- **Texture Quality**: Full Res
- **Anisotropic Filtering**: Per Texture or Forced On
- **Texture Streaming Memory Budget**: Set between **2–4 GB** (you have ample VRAM)

### Rendering Pipeline
- Consider using a Scriptable Render Pipeline:
  - **URP**: Balanced visuals + performance; great default choice
  - **HDRP**: Higher-fidelity visuals; suitable for high-end targets with your GPU

### Editor Performance
- Edit → Preferences → GI Cache: increase cache size to **50–100 GB** (if storage allows)
- Window → Rendering → Lighting → Lightmapping:
  - Enable **GPU Progressive Lightmapper**
- Window → General → Console:
  - Enable **Collapse** to reduce UI overhead

### NVIDIA-Specific Optimizations (nvidia-settings)
- Ensure you are on the latest proprietary **NVIDIA driver**
- In `nvidia-settings`:
  - PowerMizer → **Prefer Maximum Performance** for Unity
  - OpenGL Settings → **Allow Flipping**: Enabled
  - X Server Display Configuration → Advanced → **Force Full Composition Pipeline**: OFF (to reduce latency)

### Memory Management
- Quality Settings → **Texture Streaming Memory Budget**: increase to **2–4 GB**
- Player Settings → Other Settings → **Compress Meshes**: Enable (saves VRAM for textures)

### Quick Checklist
- [ ] Vulkan set as primary Graphics API (OpenGL as optional fallback)
- [ ] GPU Skinning enabled
- [ ] Texture Quality at Full Res
- [ ] Anisotropic Filtering Per Texture or Forced On
- [ ] Texture Streaming Budget at 2–4 GB
- [ ] URP or HDRP selected (as project needs)
- [ ] GI Cache set to 50–100 GB
- [ ] GPU Progressive Lightmapper enabled
- [ ] Console Collapse enabled
- [ ] NVIDIA driver up to date; Prefer Maximum Performance set
- [ ] Allow Flipping ON; Force Full Composition Pipeline OFF
- [ ] Compress Meshes enabled


