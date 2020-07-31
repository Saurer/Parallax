var io = io || {};

io.saveFile = (data, name, type) => {
  const file = new Blob([data], { type: type });
  if (window.navigator.msSaveOrOpenBlob)
    // IE10+
    window.navigator.msSaveOrOpenBlob(file, name);
  else {
    // Others
    const a = document.createElement("a");
    const url = URL.createObjectURL(file);

    a.href = url;
    a.download = name;

    document.body.appendChild(a);
    a.click();

    setTimeout(function () {
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }, 0);
  }
};
