# https://github.com/boyter/scc
scc -i cs `
    --exclude-dir Migrations `
    --no-size --no-cocomo --no-complexity -s code
    | Out-String 
    | ForEach-Object {$_ -replace "ΓöÇ", "—"}
    | Out-File sloc.txt