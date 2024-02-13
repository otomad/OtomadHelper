cd docs
make clean && make html && sphinx-build -b html -D language=zh_CN ./source/ build/html/zh_CN
