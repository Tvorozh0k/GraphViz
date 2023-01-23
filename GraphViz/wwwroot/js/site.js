function read(text) {
    // данные из файла, разделенные по строкам
    var data = text.split('\r\n');

    // ссылка на номер строки файла с данными
    var pos = 0;

    // количество кластеров (различных цветов кластеров) + сдвиг на одну строку
    var colors = parseInt(data[pos].split(' ')[1]); ++pos;

    // настройка цветов кластеров
    var groups = {};

    for (var i = 1; i <= colors; ++i) {

        // цвет зависит от рандомного числа 
        var num = Math.random() * 360;
        var col1 = "hsl(" + num + ", 100%, 75%)"; // цвет границы
        var col2 = "hsl(" + num + ", 75%, 90%)"; // цвет заднего фона

        groups[i.toString()] = {
            color: {
                border: col1,
                background: col2
            }
        };
    }

    // настройка опций - кластеров
    var options = {
        groups: groups
    };

    // число вершин в графе + сдвиг на одну строку 
    var n = parseInt(data[pos].split(' ')[1]); ++pos;

    // вершины 
    var nodes = [];

    for (var i = pos; i - pos < n; ++i) {

        // id вершины, подпись и кластер, которому принадлежит
        nodes.push({ id: data[i].split(' ')[0], label: data[i].split(' ')[0], group: data[i].split(' ')[1] });
    }

    // сдвигаем указатель на n 
    pos += n;

    // число дуг графа + сдвиг на одну строку
    var m = parseInt(data[pos].split(' ')[1]); ++pos;

    // ребра 
    var edges = [];

    for (var i = pos; i - pos < m; ++i) {

        // id ребра откуда и куда + тип ребра (дуга)
        edges.push({ id: (i - pos).toString(), from: data[i].split(' ')[0], to: data[i].split(' ')[1], arrows: "to" });
    }

    // соединяем js и html
    var container = document.getElementById('mynetwork');

    // данные графа
    var data = { nodes: nodes, edges: edges };

    // создаем график 
    var network = new vis.Network(container, data, options);

    // формируем реакцию на нажатие кластеров
    network.on("selectNode", function (params) {
        if (params.nodes.length == 1) {
            if (network.isCluster(params.nodes[0]) == true) {
                network.openCluster(params.nodes[0]);
            }
        }
    });

    // формируем кластеры 
    for (var i = 1; i <= colors; ++i) {
        var options = {
            joinCondition: function (nodeOptions) {
                return nodeOptions.group === i.toString();
            },
            clusterNodeProperties: {
                id: "cluster:" + i.toString(),
                color: groups[i.toString()].color,
                label: "cluster:" + i.toString(),
                allowSingleNodeCluster: true
            }
        };

        network.clustering.cluster(options);
    }
}

fetch('./dataViz.txt')
    .then(response => response.text())
    .then(text => read(text))
