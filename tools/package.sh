rm -rf Assemblies

tools/build.bat

if [ ! -d "Assemblies" ]; then
  echo FAIL
  exit
fi

rm -rf CraftingHysteresis
mkdir CraftingHysteresis
cp -r About Assemblies Defs CraftingHysteresis


fname=CraftingHysteresis-`git describe --tags`.zip
rm $fname
zip -r -9 $fname CraftingHysteresis

rm -rf CraftingHysteresis
