#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectWrapper : IProject
    {
        private readonly Project _project;

        public ProjectWrapper(string path)
        {
            _project = new Project(path, null, null, ProjectCollection.GlobalProjectCollection,
                ProjectLoadSettings.IgnoreMissingImports);
        }

        public void AddItem(string itemType, string itemInclude)
        {
            _project.AddItem(itemType, itemInclude);
        }

        public ICollection<ISyncProjectItem> GetItems(string itemType)
        {
            return _project.GetItems(itemType).Select(x => new SyncProjectItem(x)).ToArray();
        }

        public void RemoveItem(ISyncProjectItem item)
        {
            _project.RemoveItem(item.InnerItem);
        }

        public string FullPath
        {
            get { return _project.FullPath; }
        }

        public void Save()
        {
            _project.Save();
        }
    }

    public class SyncProjectItem : ISyncProjectItem
    {
        public SyncProjectItem(ProjectItem projectItem)
        {
            InnerItem = projectItem;
        }

        public ProjectItem InnerItem { get; private set; }

        public string EvaluatedInclude
        {
            get { return InnerItem.EvaluatedInclude; }
        }
    }
}