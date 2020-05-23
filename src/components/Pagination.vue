<template>
    <div class="pagination-block">
        <ul>

            <li v-bind:class="{ 'disabled': links.previous === undefined }" class="page-link">
                <a href="#" aria-label="Previous" @click.prevent="prevPage()">
                    <span aria-hidden="true">&laquo;</span>
                </a>
            </li>

            <li v-if="pagination.currentPage > 1">
                <a href="#" aria-label="Previous" @click.prevent="setPage(1)" class="page-link">
                    <span aria-hidden="true">First</span>
                </a>
            </li>

            <li v-if="pagination.currentPage > 1">
                <a href="#" aria-label="Next" @click.prevent="setPage(from)" class="page-link">
                    <span aria-hidden="true">...</span>
                </a>
            </li>

            <li v-for="(page, index) in pages" :class="getPageClass(page)" :key="index">
                <a href="#" @click.prevent="setPage(page)" class="page-link">{{ page }}</a>
            </li>

            <li v-if="pagination.currentPage < pagination.totalPages">
                <a href="#" aria-label="Next" @click.prevent="setPage(to)" class="page-link">
                    <span aria-hidden="true">...</span>
                </a>
            </li>

            <li v-if="pagination.currentPage < pagination.totalPages">
                <a href="#" aria-label="Previous" @click.prevent="setPage(pagination.totalPages)" class="page-link">
                    <span aria-hidden="true">Last</span>
                </a>
            </li>

            <li v-bind:class="{ 'disabled': links.next === undefined }">
                <a href="#" aria-label="Next" @click.prevent="nextPage()" class="page-link">
                    <span aria-hidden="true">&raquo;</span>
                </a>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: ['callback', 'pagination'],
        data: function () {
            return {
                link: 1,
                visible: 1,
                offset: 4,
                from: null,
                to: null,
            }
        },

        computed: {
            pages: function () {
                var self = this
                self.visible = 1
                var from = this.pagination.currentPage - this.offset
                if (from < 1) {
                    from = 1
                }
                var to = from + (this.offset * 2)
                if (to >= this.pagination.totalPages) {
                    to = this.pagination.totalPages
                }
                self.from = from
                self.to = to
                var pagesData = []
                while (from <= to) {
                    pagesData.push(from)
                    from++
                }
                if (pagesData.length === 1) {
                    self.visible = 0
                }
                return pagesData
            },
            links: function () {
                return this.pagination.links || {}
            }
        },

        ready: function () {
        },

        methods: {
            nextPage: function () {
                if (this.pagination.links.next === undefined) {
                    return false
                }
                this.link = this.pagination.currentPage + 1
                this.getData()
            },

            prevPage: function () {
                if (this.pagination.links.prev === undefined) {
                    return false
                }
                this.link = this.pagination.currentPage - 1
                this.getData()
            },

            setPage: function (page) {
                this.link = page
                this.getData()
            },

            getData: function () {
                this.callback(this.link)
            },
            getPageClass: function (page) {
                return this.pagination.currentPage === page ? 'page-numbers active current' : 'page-numbers'
            }
        }
    }

</script>

<style type="text/css" lang="scss">
    .page-item.active .page-link {
        background-color: #B79268;
        border-color: #B79268;
    }

    .page-link, .page-link:focus {
        color: #B79268;
    }
</style>
