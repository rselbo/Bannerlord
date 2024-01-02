import logging
from sys import argv, exit
from os import path, makedirs
from shutil import copy2

version = 'v1.0.0.*'

def main():
    modname = argv[1]
    sourcedir = argv[2]
    targetdir = argv[3]

    logging.basicConfig(format=f"deploy -> {modname}:%(levelname)s:%(message)s", level=logging.INFO)
    logging.info(f"Starting {modname} deploy")
    makedirs(path.join(targetdir, modname, "bin", "Win64_Shipping_Client"), exist_ok=True)

    submodule = path.join(modname, "SubModule.xml")
    targetmod = path.join(targetdir, modname)
    if not path.exists(submodule):
        logging.error(f"Submodule '%s' does not exist", submodule)
        exit(1)
    if not path.exists(sourcedir+".dll"):
        logging.error(f"Sourcedir '%s'.dll does not exist", sourcedir)
        exit(1)
    if not path.exists(targetmod):
        logging.error(f"Targetdir '%s' does not exist", targetmod)
        exit(1)

    target_submodule = path.join(targetmod, "SubModule.xml")
    logging.info(f"Writing {target_submodule}")
    with open(submodule) as f:
        data = f.read()
        data = data.replace("$VERSION", version)
        with open(target_submodule, 'w+') as t:
            t.write(data)

    targetdlldir = path.join(targetdir, modname, "bin/Win64_Shipping_Client")
    for ext in {"dll", "pdb"}:
        sourcefile = sourcedir + "." + ext
        logging.info(f"Copying {sourcefile}")
        copy2(sourcefile, targetdlldir)


if __name__ == '__main__':
    main()