# creates build/html
rm -r build -errorAction ignore
$d = mkdir build
$d = mkdir build/html
cp -r winjs.todo/Content build/html/
cp -r winjs.todo/Resources build/html/
cp -r winjs.todo/*.html build/html/


