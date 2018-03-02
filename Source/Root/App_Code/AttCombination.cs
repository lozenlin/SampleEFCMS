using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Common.LogicObject;
using Common.DataAccess.EF.Model;

namespace Att
{
    public class AttCombination
    {
        protected List<AttInfo> attList = new List<AttInfo>();

        public AttCombination(List<AttachFileForFrontend> attachFiles)
        {
            Initialize(attachFiles);
        }

        public List<AttInfo> GetList()
        {
            return attList;
        }

        protected void Initialize(List<AttachFileForFrontend> attachFiles)
        {
            foreach (AttachFileForFrontend attFile in attachFiles)
            {
                string attSubject = attFile.AttSubject;
                int sortNo = attFile.SortNo.Value;
                string fileSavedName = attFile.FileSavedName;

                //找出同名AttInfo
                AttInfo curAttInfo = attList.Find(x => x.AttSubject == attSubject);

                if (curAttInfo == null)
                {
                    curAttInfo = new AttInfo();
                    curAttInfo.AttSubject = attSubject;
                    curAttInfo.SortNo = sortNo;

                    attList.Add(curAttInfo);
                }

                string ext = Path.GetExtension(fileSavedName);
                DateTime? mdfDate = attFile.MdfDate;

                if (!mdfDate.HasValue)
                {
                    mdfDate = attFile.PostDate.Value;
                }

                // add file data
                FileData curFile = new FileData()
                {
                    AttId = attFile.AttId,
                    SortNo = attFile.SortNo.Value,
                    FileName = fileSavedName,
                    FileExt = ext,
                    FileSize = attFile.FileSize,
                    ReadCount = attFile.ReadCount,
                    MdfDate = mdfDate.Value
                };

                if (curFile.FileSize > 1024)
                {
                    curFile.FileSizeDesc = string.Format("{0:#,0.##} MB", curFile.FileSize / 1024f);
                }
                else
                {
                    curFile.FileSizeDesc = string.Format("{0:#,0} KB", curFile.FileSize);
                }

                if (curAttInfo.SortNo > curFile.SortNo)
                {
                    curAttInfo.SortNo = curFile.SortNo;
                }

                curAttInfo.Files.Add(curFile);
            }

            // attList.Sort((x, y) => x.SortNo.CompareTo(y.SortNo));
        }
    }

    public class AttInfo
    {
        public string AttSubject;
        public int SortNo = -1;
        public List<FileData> Files = new List<FileData>();
    }

    public class FileData
    {
        public Guid AttId;
        public int SortNo;
        public string FileName;
        public string FileExt;
        public int FileSize;
        public string FileSizeDesc;
        public int ReadCount;
        public DateTime MdfDate;
    }

}