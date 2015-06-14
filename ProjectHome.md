A super-simple C# program to dump the content of a KeePass file (`*`.kdb, `*`.kdbx, etc.) to terminal.

Uses code from the KeePass project, therefore supports all file types which KeePass supports (though, only kdbx has been tested).

Inspired by Mono's inability to run KeePass on MacOsX (System.EntryPointNotFoundException: GdipCreateFromContext\_macosx)

Includes an unmodified KeePassLib from KeePass 2.16 for convenience.
Limitations: unlocking a file is only possible with a passphrase (not key-files, etc.). KeePassLib provides these features, it's just I haven't exposed them because of laziness.

Caveats: anything you print to a terminal may remain in the computer's memory even after you close the terminal.

### "Screenshot": ###
```
test
    Sample Entry     User Name             >Password<  
```