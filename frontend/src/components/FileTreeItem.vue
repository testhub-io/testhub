<template>
  <div id="file-tree-item"
    :style="{ transform: `translate(${this.depth + 35}px)` }" >
    <div
      class="label-wrapper"
      @click="handleClick"
      >
      <b-row>
        <b-col sm="1">
          <b-row>
           <b-col sm="6">
              <b-icon
                aria-hidden="true"
                :style="{ color: '#000' }"
                :icon="nodeIcon">
              </b-icon>
            </b-col>
           <b-col sm="6">
             <b-badge :variant="`${coverageBadgeColor()}`">{{ coverage }}</b-badge>
           </b-col>
          </b-row>
        </b-col>
        <b-col sm="9"><span class="path">{{ node.name }}</span></b-col>
      </b-row>
    </div>
    <div v-for="child in node.children" :key="child.name">
      <file-tree-item

        :node="child"
        v-if="expandDirectory"
        :children="child.children"
        :depth="depth + 1"
      ></file-tree-item>
    </div>
  </div>
</template>

<script>

export default {
  name: 'file-tree-item',

  props: {
    node: { type: Object, required: true },
    depth: { type: Number, required: true }
  },

  components: {},

  data() {
    return {
      expandDirectory: false,
    }
  },

  computed: {
    coverage() {
      return Math.round(
        (parseFloat(this.node.data['line-rate']) * 100))
    },

    nodeIcon() {
      let icon;

      if (this.node.children.length > 0) {
        icon = this.expandDirectory ? 'folder-minus' : 'folder-plus'
      } else {
        icon = 'file-code'
      }

      return icon
    },
  },

  methods: {
    toggleFolder() { this.expandDirectory = !this.expandDirectory; },

    coverageBadgeColor() {
      if(this.coverage >= 0 && this.coverage < 75) {
        return 'danger'
      } else if (this.coverage > 75 && this.coverage < 85) {
        return 'warning'
      } else {
        return 'success'
      }
    },

    viewFile() {
      this.$store.dispatch('setCodeFile', this.node)
    },

    handleClick() {
      this.fileLink() ? this.viewFile() : this.toggleFolder()
    },

    fileLink() {
      return !this.node.children.length > 0
    },

    indent() {
      return { transform: `translate(${this.depth * 50}px)` }
    }
  },

  mounted() {
  }
}

</script>

<style>
.label-wrapper {
  margin-bottom: 5px;
}

.label-wrapper:hover {
  cursor: pointer;
}

.cov-badge {

}

.cov-badge {
  text-align: center;
  color: #fff;
}

.highCov{
  background-color: rgba(95,151,68, 1);
}

.lowCov{
  background-color: rgba(185,73,71, 1);
}

.mediumCov{
  background-color: #F89406
}
</style>
