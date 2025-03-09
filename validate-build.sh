for f in $(find ./ -name '*.csproj'); do 
  echo $f;
  dotnet build $f;
  status=$?
  [ $status -eq 0 ] && echo "No errors found" || exit $status
done