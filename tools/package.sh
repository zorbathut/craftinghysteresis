rm -rf Assemblies

dirname=${PWD##*/}
projname=${dirname%%_Dev}

if [ -f "$projname.sln" ]; then
  rm -rf Assemblies
  
  tools/build.bat $projname

  if [ ! -f "Assemblies/$projname.dll" ]; then
    echo FAIL
    exit
  fi
fi

rm -rf $projname
mkdir $projname
cp -r About Assemblies Defs Languages $projname
sed "s/ Dev//" -i $projname/About/About.xml


fname=$projname-`git describe --tags`.zip
rm $fname
zip -r -9 $fname $projname

rm -rf $projname

rm -rf ../$projname
unzip $fname -d ..
