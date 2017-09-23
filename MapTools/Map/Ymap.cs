﻿using MapTools.Types;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MapTools.Map
{
    public class Ymap
    {
        public string filename { get; set; }
        public CMapData CMapData { get; set; }

        public Ymap(string name)
        {
            filename = name;
            CMapData = new CMapData(name);
        }

        public XDocument WriteXML()
        {
            //document
            XDocument doc = new XDocument();
            //declaration
            XDeclaration declaration = new XDeclaration("1.0", "UTF-8", "no");
            doc.Declaration = declaration;
            //CMapData
            doc.Add(CMapData.WriteXML());
            return doc;
        }

        public Ymap(XDocument document, string name)
        {
            filename = name;
            CMapData = new CMapData(document.Element("CMapData"));
        }

        public static Ymap Merge(Ymap[] list)
        {
            if (list == null || list.Length < 1)
                return null;
            Ymap merged = new Ymap("merged");
            foreach (Ymap current in list)
            {
                if (current.CMapData.entities != null && current.CMapData.entities.Count > 0)
                {
                    foreach (CEntityDef entity in current.CMapData.entities)
                    {
                        if (!merged.CMapData.entities.Contains(entity))
                            merged.CMapData.entities.Add(entity);
                        else
                            Console.WriteLine("Skipped duplicated CEntityDef " + entity.guid);
                    }
                }

                if (current.CMapData.instancedData.GrassInstanceList != null && current.CMapData.instancedData.GrassInstanceList.Count > 0)
                {
                    foreach (GrassInstance instance in current.CMapData.instancedData.GrassInstanceList)
                    {
                        if (!merged.CMapData.instancedData.GrassInstanceList.Contains(instance))
                            merged.CMapData.instancedData.GrassInstanceList.Add(instance);
                        else
                            Console.WriteLine("Skipped duplicated GrassInstance Item " + instance.archetypeName);
                    }
                }
            }
            return merged;
        }
    }
}
