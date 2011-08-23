/*
  Dump KeePass Passwords to terminal
  Copyright (C) 2011 Carmi Grushko (carmi.grushko@gmail.com)

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

/* --- Uses KeePassLib from the sources of KeePass 2.16 --- */

using System;
using KeePassLib;
using KeePassLib.Serialization;
using KeePassLib.Keys;
using KeePassLib.Interfaces;
using KeePassLib.Collections;

namespace DumpKeePassFile
{
	public sealed class CoutLogger : IStatusLogger
	{
		public void StartLogging(string strOperation, bool bWriteOperationToLog)
		{
			System.Console.WriteLine(strOperation);
		}
		
		public void EndLogging() { }
		
		public bool SetProgress(uint uPercent) { return true; }
		public bool SetText(string strNewText, LogStatusType lsType) 
		{ 
			System.Console.WriteLine(strNewText);
			return true; 
		}
		public bool ContinueWork() { return true; }
	}
	
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 2)
			{
				ShowUsage();
				return;
			}
			
			PwDatabase db = new PwDatabase();
			IOConnectionInfo source = new IOConnectionInfo();
			source.Path = args[1];
			
			string password = args[0];
			CompositeKey key = new CompositeKey();
			key.AddUserKey(new KcpPassword(password));
			
			try
			{
				db.Open(source, key, new CoutLogger());
				PrintGroups(db.RootGroup, 0);
			}
			catch (System.IO.FileNotFoundException e)
			{
				Console.WriteLine(e.Message);
			}
			catch (System.FormatException e)
			{
				Console.WriteLine(e.Message);
			}
			catch (KeePassLib.Keys.InvalidCompositeKeyException)
			{
				Console.WriteLine("Invalid password for KeePass file {0}", args[1]);
			}
		}
		
		private static void Indent(int level)
		{
			for (int i=0; i<level; i++)
				System.Console.Write("  ");
		}
		
		private static void PrintEntries(PwObjectList<PwEntry> Entries, int level)
		{
			foreach (PwEntry entry in Entries)
			{
				Indent(level);
				System.Console.WriteLine("{0,-15}  {1,-20}  {2,-20}", 
				                         entry.Strings.Get("Title").ReadString(),
				                         entry.Strings.Get("UserName").ReadString(),
				                         ">"+entry.Strings.Get("Password").ReadString()+"<");
			}
		}
		
		private static void PrintGroups(PwGroup group, int level)
		{
			if (group.Entries.UCount == 0 && 
			    group.Groups.UCount == 0 )
				return;
			
			Indent(level);
			System.Console.WriteLine(group.Name);
			
			PrintEntries(group.Entries, level+2);
			
			if (group.Groups.UCount == 0 )
				return;
						
			foreach (PwGroup subgroup in group.Groups)
			{
				Indent(level);
				PrintGroups(subgroup, level+1);
			}
		}
		
		private static void ShowUsage()
		{
			System.Console.Write(
@"
Dump KeePass Files 
------------------
Usage: 
DumpKeePassFile.exe PASSWORD FILE

Can only dump files which are locked by passwords only.

If using in Linux/MacOsX - usually 'mono'
should be added before the above command.
"
			                         );
		}
	}
}

