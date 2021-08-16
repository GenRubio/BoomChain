<style>
    .settings-button {
        font-size: 38px;
        position: absolute;
        margin-left: 946px;
        margin-top: 16px;
        height: 50px;
        width: 50px;
        background-color: #ffffff78;
        border-radius: 10px;
        cursor: pointer;
        color: #1622ad9e;
    }

    .icon-settings {
        margin-left: 6px;
        margin-top: -2px;
    }

</style>
<div class="settings-button none" data-toggle="tooltip" data-placement="left" title="Ajustes">
    <div class="icon-settings">
        <i class="fas fa-cog"></i>
    </div>
</div>

@include('launcher.components.flowerPower.modal-settings')