<template>
  <b-container fluid id="file-tree" :style="{ 'margin-top': '30px' }">
    <TreeItem
      v-for="node in tree"
      :node="node"
      :key="node.name"
      :depth="0"
      :children="node.children">
    </TreeItem>
  </b-container>
</template>

<script>
import TreeItem from './FileTreeItem';

export default {
  name: 'file-tree',

  props: {
    filesData: {
      type: Array,
      required: true
    }
  },

  components: { TreeItem },

  data() {
    return {
      paths: [],
      tree: null
    };
  },

  methods: {
    pathExists(array, key, value) {
      let counter = 0

      while (counter <array.length && array[counter][key] !== value) {
        counter++
      }

      const exists = counter < array.length ?
        array[counter] :
        false

      return exists;
    },

    generateTree() {
      const paths = this.filesData.map(file => {
        const pathName = file.filename || file.name
        return [pathName.split('/'), file]
      }).filter(path => {
        const cls = path[1].name.split('/')
        return cls[cls.length-1] !== '<>c'
      })


      let tree = [];

      for (let i = 0; i < paths.length; i++) {

        const path = paths[i][0];
        let level = tree;

        const nextPath = i < paths.length - 1 ? paths[i + 1][0] : null

        if (nextPath && path.length === nextPath.length) {
          const matchesNext = path
            .every((value, idx) => value === nextPath[idx])

          if (matchesNext) {
            const current = paths[i][1]
            const next = paths[i + 1][1]

            const avg = (a, b) => (parseFloat(a) + parseFloat(b)) / 2

            const combinedData = {
              linesData: [...current.linesData, ...next.linesData],
              filename: current.filename,
              'line-rate': `${avg(current['line-rate'], next['line-rate'])}`,
              'branch-rate': `${avg(current['branch-rate'], next['branch-rate'])}`
            }
            paths[i].splice(1, 1, combinedData)
          }
        }

        for (let j = 0; j < path.length; j++) {
          const part = path[j];

          const existingPath = this.pathExists(level, 'name', part);

          if (existingPath) {
            level = existingPath.children;
          } else {
            const newPart = {
                name: part,
                children: [],
                data: paths[i][1]
            }

            level.push(newPart);
            level = newPart.children;
          }
        }
      }

      this.tree = tree
    }
  },

  mounted() {
    this.generateTree()
  }
}
</script>

<style>

</style>
