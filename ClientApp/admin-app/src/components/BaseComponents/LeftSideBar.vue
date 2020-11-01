<template>
  <div class="left-side-bar">
    <div class="brand-logo">
      <a href="/">
        <img src="vendors/images/favicon-32x32.png" alt="" class="dark-logo" />
        <img src="vendors/images/favicon-32x32.png" alt="" class="light-logo" />
        <span class="ml-1" style="color: blue"> App Music</span>
      </a>
      <div class="close-sidebar" data-toggle="left-sidebar-close">
        <i class="ion-close-round"></i>
      </div>
    </div>
    <div class="menu-block customscroll">
      <div class="sidebar-menu">
        <ul id="accordion-menu">
          <li>
            <a href="/" class="dropdown-toggle no-arrow">
              <span class="micon dw dw-house-1"></span
              ><span class="mtext">Home</span>
            </a>
          </li>
          <li>
            <a href="calendar.html" class="dropdown-toggle no-arrow">
              <span class="micon dw dw-calendar1"></span
              ><span class="mtext">Calendar</span>
            </a>
          </li>
          <li class="dropdown">
            <a href="javascript:;" class="dropdown-toggle">
              <span class="micon dw dw-apartment"></span
              ><span class="mtext"> UI Elements </span>
            </a>
            <ul class="submenu">
              <li><a href="ui-buttons.html">Buttons</a></li>
              <li><a href="ui-cards.html">Cards</a></li>
              <li><a href="ui-cards-hover.html">Cards Hover</a></li>
              <li><a href="ui-modals.html">Modals</a></li>
              <li><a href="ui-tabs.html">Tabs</a></li>
              <li>
                <a href="ui-tooltip-popover.html">Tooltip &amp; Popover</a>
              </li>
              <li><a href="ui-sweet-alert.html">Sweet Alert</a></li>
              <li><a href="ui-notification.html">Notification</a></li>
              <li><a href="ui-timeline.html">Timeline</a></li>
              <li><a href="ui-progressbar.html">Progressbar</a></li>
              <li><a href="ui-typography.html">Typography</a></li>
              <li><a href="ui-list-group.html">List group</a></li>
              <li><a href="ui-range-slider.html">Range slider</a></li>
              <li><a href="ui-carousel.html">Carousel</a></li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  setup() {
    return {};
  },
});
</script>

<style scoped>
</style>