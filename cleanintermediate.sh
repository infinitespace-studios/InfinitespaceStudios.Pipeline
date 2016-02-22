#!/bin/bash
find . -name "obj" -print0 | xargs -0 rm -Rf
find . -name "bin" -print0 | xargs -0 rm -Rf
find . -name "packages" -print0 | xargs -0 rm -Rf
find . -type f -name '*.o' | xargs rm
