var inputFile = inputFile || {};

inputFile.readFile = async (file) => {
  const reader = new FileReader();

  return new Promise((resolve) => {
    reader.addEventListener("load", () => {
      const [_, data] = reader.result.split(",");
      return resolve({
        name: file.name,
        data,
      });
    });

    reader.readAsDataURL(file);
  });
};

inputFile.handleChange = async function (reference, element) {
  const tasks = [];
  for (const file of element.files) {
    tasks.push(inputFile.readFile(file));
  }

  const result = await Promise.all(tasks);
  await reference.invokeMethodAsync("HandleIncomingFiles", result);
  reference.dispose();
};

inputFile.clear = function (element) {
  element.value = "";
};

inputFile.click = function (element) {
  element.click();
};
