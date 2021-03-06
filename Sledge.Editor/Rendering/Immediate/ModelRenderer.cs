using System.Linq;
using OpenTK.Graphics.OpenGL;
using Sledge.DataStructures.Models;
using Sledge.Graphics.Helpers;

namespace Sledge.Editor.Rendering.Immediate
{
    public static class ModelRenderer
    {
        public static void Render(Model model)
        {
            var transforms = model.Bones.Select(x => x.Transform).ToList();

            GL.Color4(1f, 1f, 1f, 1f);
            TextureHelper.EnableTexturing();

            foreach (var group in model.Meshes.GroupBy(x => x.SkinRef))
            {
                var texture = model.Textures[group.Key].TextureObject;
                if (texture != null) texture.Bind();
                foreach (var mesh in group)
                {
                    GL.Begin(BeginMode.Triangles);
                    foreach (var v in mesh.Vertices)
                    {
                        var transform = transforms[v.BoneWeightings.First().Bone.BoneIndex];
                        var c = v.Location * transform;
                        if (texture != null)
                        {
                            GL.TexCoord2(v.TextureU, v.TextureV);
                        }
                        GL.Vertex3(c.X, c.Y, c.Z);
                    }
                    GL.End();
                }
                if (texture != null) texture.Unbind();
            }
            TextureHelper.DisableTexturing();
        }
    }
}