//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrcStudio.Project;

namespace CrcStudio.Utility
{
    public class MruMenuManager
    {
        private const string ClearItemsText = "Clear Recent Items List";
        private readonly Action<MruMenuManager, string> _callbackMethod;
        private readonly string _fileName;
        private readonly int _maxItemsSaved;
        private readonly ToolStripMenuItem _menuStripItem;
        private readonly List<string> _mruFiles;
        private readonly ToolStripMenuItem _parentMenuStripItem;

        public MruMenuManager(string name, int numberOfItems, ToolStripMenuItem parentMenuStripItem,
                              ToolStripMenuItem menuStripItem, Action<MruMenuManager, string> callbackMethod)
        {
            Name = name;
            NumberOfItems = numberOfItems;
            _maxItemsSaved = 3*numberOfItems;
            _fileName = Path.Combine(CrcsSettings.Current.AppDataPath, name + ".mru");
            _parentMenuStripItem = parentMenuStripItem;
            _menuStripItem = menuStripItem;
            _callbackMethod = callbackMethod;
            _parentMenuStripItem.DropDownOpening += MenuStripItemDropDownOpening;
            _mruFiles = new List<string>(FileUtility.ReadAllLines(_fileName));
            CreateMenu();
        }

        public string Name { get; private set; }
        public int NumberOfItems { get; set; }
        public bool Visible { get { return _mruFiles.Count > 0; } }

        private void CreateMenu()
        {
            _menuStripItem.DropDownItems.Clear();
            if (_mruFiles.Count == 0)
            {
                _menuStripItem.Visible = false;
                return;
            }
            _menuStripItem.Visible = true;
            int count = 1;
            foreach (string file in _mruFiles)
            {
                if (!File.Exists(file)) continue;
                CreateMenuItem(file);
                count++;
                if (count >= NumberOfItems) break;
            }
            CreateMenuItem(ClearItemsText);
        }

        private void CreateMenuItem(string file)
        {
            var item = new ToolStripMenuItem(CreateMenuText(file));
            item.Tag = file;
            item.Click += ItemClick;
            _menuStripItem.DropDownItems.Add(item);
        }

        private string CreateMenuText(string file)
        {
            // Max 60 chars;
            if (file.Length <= 70) return file;
            string[] fileArray = file.Split('\\');
            string fileText;
            int startIndex = 2;
            do
            {
                fileText = string.Format("{0}\\...\\{1}", fileArray[0],
                                         string.Join("\\", fileArray, startIndex, fileArray.Length - startIndex));
                startIndex++;
            } while (fileText.Length > 70 && startIndex <= fileArray.Length);
            return fileText;
        }

        private void ItemClick(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null) return;
            var file = item.Tag as string;
            if (file == ClearItemsText)
            {
                _mruFiles.Clear();
                Save();
                return;
            }
            if (_callbackMethod != null && !string.IsNullOrWhiteSpace(file))
            {
                _callbackMethod(this, file);
            }
        }

        private void MenuStripItemDropDownOpening(object sender, EventArgs e)
        {
            CreateMenu();
        }

        private void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            FileUtility.SaveAllLines(_fileName, _mruFiles, Encoding.UTF8);
        }

        public void Add(string fileSystemPath)
        {
            if (_mruFiles.Contains(fileSystemPath))
            {
                _mruFiles.Remove(fileSystemPath);
            }
            _mruFiles.Insert(0, fileSystemPath);
            if (_mruFiles.Count > _maxItemsSaved)
                _mruFiles.RemoveRange(_maxItemsSaved, _mruFiles.Count - _maxItemsSaved);
            Save();
        }
    }
}