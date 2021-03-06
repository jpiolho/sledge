using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Sledge.Common;
using Sledge.DataStructures.Geometric;

namespace Sledge.DataStructures.Models
{
    public class Model: IDisposable
    {
        public string Name { get; set; }
        public List<Bone> Bones { get; private set; }
        public List<Mesh> Meshes { get; private set; }
        public List<Animation> Animations { get; private set; }
        public List<Texture> Textures { get; set; }
        public bool BonesTransformMesh { get; set; }
        private bool _preprocessed;

        public Model()
        {
            Bones = new List<Bone>();
            Meshes = new List<Mesh>();
            Animations = new List<Animation>();
            Textures = new List<Texture>();
            _preprocessed = false;
        }

        /// <summary>
        /// Preprocess the model for rendering purposes.
        /// Normalises the texture coordinates,
        /// pre-computes chrome texture values, and
        /// combines all the textures into a single bitmap.
        /// </summary>
        public void PreprocessModel()
        {
            if (_preprocessed) return;
            _preprocessed = true;

            PreCalculateChromeCoordinates();
            CombineTextures();
            NormaliseTextureCoordinates();
        }

        /// <summary>
        /// Combines the textures in this model into one bitmap and modifies all the referenced skins and texture coordinates to use the combined texture.
        /// This modifies the model object.
        /// </summary>
        private void CombineTextures()
        {
            // Calculate the dimension of the combined texture
            var width = 0;
            var height = 0;
            var heightList = new Dictionary<int, int>();
            foreach (var texture in Textures)
            {
                width = Math.Max(texture.Width, width);
                heightList.Add(texture.Index, height);
                height += texture.Height;
            }

            // Create the combined texture and draw all the textures onto it
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                var y = 0;
                foreach (var texture in Textures)
                {
                    g.DrawImage(texture.Image, 0, y);
                    y += texture.Height;
                }
            }

            // Create the texture object and replace the existing textures
            var tex = new Texture
            {
                Flags = Textures[0].Flags,
                Height = height,
                Width = width,
                Image = bmp,
                Index = 0,
                Name = "Combined Texture"
            };
            foreach (var texture in Textures)
            {
                texture.Image.Dispose();
            }
            Textures.Clear();
            Textures.Insert(0, tex);

            // Update all the meshes with the new texture and alter the texture coordinates as needed
            foreach (var mesh in Meshes)
            {
                if (!heightList.ContainsKey(mesh.SkinRef))
                {
                    mesh.SkinRef = -1;
                    continue;
                }
                var i = mesh.SkinRef;
                var yVal = heightList[i];
                foreach (var v in mesh.Vertices)
                {
                    v.TextureV += yVal;
                }
                mesh.SkinRef = 0;
            }
            // Reset the texture indices
            for (var i = 0; i < Textures.Count; i++)
            {
                Textures[i].Index = i;
            }
        }

        /// <summary>
        /// Pre-calculates chrome texture values for the model.
        /// This operation modifies the model vertices.
        /// </summary>
        private void PreCalculateChromeCoordinates()
        {
            var transforms = Bones.Select(x => x.Transform).ToList();
            foreach (var g in Meshes.GroupBy(x => x.SkinRef))
            {
                var skin = Textures.FirstOrDefault(x => x.Index == g.Key);
                if (skin == null || (skin.Flags & 0x02) == 0) continue;
                foreach (var v in g.SelectMany(m => m.Vertices))
                {
                    var transform = transforms[v.BoneWeightings.First().Bone.BoneIndex];

                    // Borrowed from HLMV's StudioModel::Chrome function
                    var tmp = transform.Shift.Normalise();

                    // Using unitx for the "player right" vector
                    var up = tmp.Cross(CoordinateF.UnitX).Normalise();
                    var right = tmp.Cross(up).Normalise();

                    // HLMV is doing an inverse rotate (no translation),
                    // so we set the shift values to zero after inverting
                    var inv = transform.Inverse();
                    inv[12] = inv[13] = inv[14] = 0;
                    up = up * inv;
                    right = right * inv;

                    v.TextureU = (v.Normal.Dot(right) + 1) * 32;
                    v.TextureV = (v.Normal.Dot(up) + 1) * 32;
                }
            }
        }

        /// <summary>
        /// Normalises vertex texture coordinates to be between 0 and 1.
        /// This operation modifies the model vertices.
        /// </summary>
        private void NormaliseTextureCoordinates()
        {
            foreach (var g in Meshes.GroupBy(x => x.SkinRef))
            {
                var skin = Textures.FirstOrDefault(x => x.Index == g.Key);
                if (skin == null) continue;
                foreach (var v in g.SelectMany(m => m.Vertices))
                {
                    v.TextureU /= skin.Width;
                    v.TextureV /= skin.Height;
                }
            }
        }

        public void Dispose()
        {
            foreach (var t in Textures)
            {
                if (t.Image != null) t.Image.Dispose();
                if (t.TextureObject != null) t.TextureObject.Dispose();
            }
        }
    }
}